package httpctx

import (
	"fmt"
	"net/http"
	"reflect"

	"github.com/jinzhu/gorm"
)

// interfaceToDb get db object from http context
func (ctx *HTTPCtx) interfaceToDb() *gorm.DB {
	dbValue := ctx.GetVal(reflect.TypeOf(&gorm.DB{}))
	if !dbValue.IsValid() {
		return nil
	}

	db := dbValue.Interface().(*gorm.DB)
	return db
}

// DbAddOne insert one record to database
func (ctx *HTTPCtx) DbAddOne(v interface{}) {
	db := ctx.interfaceToDb()
	if db == nil {
		ctx.Error(http.StatusBadRequest, "can not find db object")
		return
	}

	err := ctx.CheckFormData(v)
	if err != nil {
		ctx.Error(http.StatusBadRequest, "the posted data is invalid,"+err.Error())
		return
	}

	err = db.Create(v).Error
	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}

	ctx.Success(http.StatusOK, v)
}

// DbUpdateOne udpate one database record
func (ctx *HTTPCtx) DbUpdateOne(v interface{}, field string) {
	id := ctx.ParamsInt(":id")
	if id == 0 {
		ctx.Error(http.StatusBadRequest, "should specified the key param")
		return
	}

	if field == "" {
		ctx.Error(http.StatusBadRequest, "should specified the key field")
		return
	}

	db := ctx.interfaceToDb()
	if db == nil {
		ctx.Error(http.StatusBadRequest, "can not find db object")
		return
	}

	err := ctx.CheckFormData(v)
	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}

	where := fmt.Sprintf("%s=?", field)
	err = db.Model(v).Where(where, id).Save(v).Error
	if err != nil {
		ctx.Error(http.StatusBadRequest, fmt.Sprintf("update database record error:%s", err.Error()))
		return
	}
	ctx.Success(http.StatusOK, v)
}

// DbDelete multiple delete record by the specified field
func (ctx *HTTPCtx) DbDelete(v interface{}, field string) {
	if field == "" {
		ctx.Error(http.StatusBadRequest, "the key field can not be null")
		return
	}

	db := ctx.interfaceToDb()
	if db == nil {
		ctx.Error(http.StatusBadRequest, "db object is null")
		return
	}

	ids, err := ctx.GetDelIDs()
	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}

	where := fmt.Sprintf("(%s in (?))", field)
	err = db.Where(where, ids).Delete(v).Error
	if err != nil {
		ctx.Error(http.StatusBadRequest, fmt.Sprintf("failed to delete records:%s", err.Error()))
		return
	}
	ctx.Success(http.StatusOK, "200")
}
