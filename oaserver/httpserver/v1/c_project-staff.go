package v1

import (
	"net/http"

	"github.com/jinzhu/gorm"
	"github.com/raysonxin/easyoa/oaserver/models"
	"github.com/raysonxin/easyoa/oaserver/toolkits/httpctx"
)

// ProjectStaffController
type ProjectStaffController struct {
}

// List
func (c *ProjectStaffController) List(ctx *httpctx.HTTPCtx, db *gorm.DB) {

	projId := ctx.QueryInt("proj")
	if projId == 0 {
		ctx.Error(http.StatusBadRequest, "request param proj lost")
		return
	}
	peroid := ctx.QueryInt("period")
	if peroid == 0 {
		ctx.Error(http.StatusBadRequest, "request param period lost")
		return
	}

	staffes := make([]*models.ProjectStaffModel, 0)
	var total int64

	err := db.Table("oa_project_staff").Where("proj_id=? and period=?", projId, peroid).Count(&total).Error
	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}

	limit, offset, pageInfo := ctx.QueryPageInfo(int(total))

	err = db.Table("oa_project_staff").
		Where("proj_id=? and period=?", projId, peroid).
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

// AddOne
func (c *ProjectStaffController) AddOne(ctx *httpctx.HTTPCtx, db *gorm.DB) {
	one := new(models.ProjectStaffModel)
	ctx.DbAddOne(one)
}

// UpdateOne
func (c *ProjectStaffController) UpdateOne(ctx *httpctx.HTTPCtx, db *gorm.DB) {
	one := new(models.ProjectStaffModel)
	ctx.DbUpdateOne(one, "id")
}

// Delete 删除
func (c *ProjectStaffController) Delete(ctx *httpctx.HTTPCtx, db *gorm.DB) {
	one := new(models.ProjectStaffModel)
	ctx.DbDelete(one, "id")
}
