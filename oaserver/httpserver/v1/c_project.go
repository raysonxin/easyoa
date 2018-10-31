package v1

import (
	"net/http"

	"github.com/jinzhu/gorm"
	"github.com/raysonxin/easyoa/oaserver/models"
	"github.com/raysonxin/easyoa/oaserver/toolkits/httpctx"
)

// ProjectController
type ProjectController struct {
}

// List
func (c *ProjectController) List(ctx *httpctx.HTTPCtx, db *gorm.DB) {

	projects := make([]*models.ProjectModel, 0)
	var total int64

	err := db.Table("oa_project").Count(&total).Error
	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}

	limit, offset, pageInfo := ctx.QueryPageInfo(int(total))

	err = db.Table("oa_project").
		Limit(limit).
		Offset(offset).
		Scan(&projects).
		Error

	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}
	ctx.SuccessPage(http.StatusOK, projects, pageInfo)
}

// AddOne
func (c *ProjectController) AddOne(ctx *httpctx.HTTPCtx, db *gorm.DB) {
	one := new(models.ProjectModel)
	ctx.DbAddOne(one)
}

// UpdateOne
func (c *ProjectController) UpdateOne(ctx *httpctx.HTTPCtx, db *gorm.DB) {
	one := new(models.ProjectModel)
	ctx.DbUpdateOne(one, "id")
}

// Delete 删除
func (c *ProjectController) Delete(ctx *httpctx.HTTPCtx, db *gorm.DB) {
	one := new(models.ProjectModel)
	ctx.DbDelete(one, "id")
}
