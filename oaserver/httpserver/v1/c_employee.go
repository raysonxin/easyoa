package v1

import (
	"net/http"

	"github.com/jinzhu/gorm"
	"github.com/raysonxin/easyoa/oaserver/models"
	"github.com/raysonxin/easyoa/oaserver/toolkits/httpctx"
)

// EmployeeController goods
type EmployeeController struct {
}

// List get employees
func (c *EmployeeController) List(ctx *httpctx.HTTPCtx, db *gorm.DB) {

	employees := make([]*models.EmployeeModel, 0)
	var total int64

	err := db.Table("oa_employee").Count(&total).Error
	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}

	limit, offset, pageInfo := ctx.QueryPageInfo(int(total))

	err = db.Table("oa_employee").
		Limit(limit).
		Offset(offset).
		Scan(&employees).
		Error

	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}
	ctx.SuccessPage(http.StatusOK, employees, pageInfo)
}

// AddOne add product
func (c *EmployeeController) AddOne(ctx *httpctx.HTTPCtx, db *gorm.DB) {
	one := new(models.EmployeeModel)
	ctx.DbAddOne(one)
}

// AddOne add product
func (c *EmployeeController) UpdateOne(ctx *httpctx.HTTPCtx, db *gorm.DB) {
	one := new(models.EmployeeModel)
	ctx.DbUpdateOne(one, "id")
}

// Delete 删除
func (c *EmployeeController) Delete(ctx *httpctx.HTTPCtx, db *gorm.DB) {
	one := new(models.EmployeeModel)
	ctx.DbDelete(one, "id")
}
