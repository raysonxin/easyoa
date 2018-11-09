package v1

import (
	"net/http"
	"time"

	"github.com/jinzhu/gorm"
	"github.com/raysonxin/easyoa/oaserver/models"
	"github.com/raysonxin/easyoa/oaserver/toolkits/httpctx"
)

// ProjectStaffController
type ProjectStaffController struct {
}

// List
func (c *ProjectStaffController) ProjectAllocations(ctx *httpctx.HTTPCtx, db *gorm.DB) {

	projId := ctx.QueryInt("proj")
	if projId == 0 {
		ctx.Error(http.StatusBadRequest, "request param proj lost")
		return
	}

	staffes := make([]*models.ProjectStaffModel, 0)
	var total int64

	err := db.Table("oa_project_staff").Where("proj_id=?", projId).Count(&total).Error
	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}

	limit, offset, pageInfo := ctx.QueryPageInfo(int(total))

	err = db.Select("ps.id,ps.proj_id,ps.ding_id,ps.start_date,emp.name,emp.job").
		Table("oa_project_staff ps").
		Joins("inner join oa_employee emp on emp.ding_id=ps.ding_id").
		Where("proj_id=?", projId).
		Limit(limit).
		Offset(offset).
		Scan(&staffes).
		Error

	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}
	ctx.SuccessPage(http.StatusOK, staffes, pageInfo)
}

// Enroll
func (c *ProjectStaffController) Enroll(ctx *httpctx.HTTPCtx, db *gorm.DB) {
	one := new(models.ProjectStaffModel)
	ctx.DbAddOne(one)
}

// Exit 删除
func (c *ProjectStaffController) Exit(ctx *httpctx.HTTPCtx, db *gorm.DB) {
	// one := new(models.ProjectStaffRecordModel)
	// ctx.DbDelete(one, "id")

	id := ctx.ParamsInt(":id")
	if id == 0 {
		ctx.Error(http.StatusBadRequest, "param:id invalid")
		return
	}

	stop := ctx.Params("stop")
	if stop == "" {
		ctx.Error(http.StatusBadRequest, "param:stop invalid")
		return
	}

	t, err := time.Parse("2006-01-02", stop)
	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}

	var item models.ProjectStaffModel
	err = db.Table("oa_project_staff").Find(&item, "id=?", id).Error
	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}

	record := models.ProjectStaffRecordModel{
		ProjId:    item.ProjId,
		DingId:    item.DingId,
		StartDate: item.StartDate,
		StopDate:  t,
	}

	tx := db.Begin()
	err = tx.Table("oa_project_staff_record").Create(&record).Error
	if err == nil {
		err = tx.Table("oa_project_staff").Delete(&models.ProjectStaffModel{}, "id=?", id).Error
	}

	if err != nil {
		tx.Rollback()
		ctx.Error(http.StatusBadRequest, err.Error())
	} else {
		tx.Commit()
		ctx.Success(http.StatusOK, "200")
	}

}

// UpdateOne
func (c *ProjectStaffController) UpdateOne(ctx *httpctx.HTTPCtx, db *gorm.DB) {
	one := new(models.ProjectStaffRecordModel)
	ctx.DbUpdateOne(one, "id")
}
