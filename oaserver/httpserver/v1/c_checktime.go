package v1

import (
	"encoding/json"
	"net/http"

	"github.com/jinzhu/gorm"
	"github.com/raysonxin/easyoa/oaserver/models"
	"github.com/raysonxin/easyoa/oaserver/toolkits/httpctx"
)

// CheckTimeController
type CheckTimeController struct {
}

// List get employees
func (c *CheckTimeController) List(ctx *httpctx.HTTPCtx, db *gorm.DB) {

	peroid := ctx.QueryInt("period")
	if peroid == 0 {
		ctx.Error(http.StatusBadRequest, "request param period lost")
		return
	}

	dingId := ctx.QueryInt("ding")
	if dingId == 0 {
		ctx.Error(http.StatusBadRequest, "request param ding lost")
		return
	}

	records := make([]*models.CheckTimeModel, 0)
	var total int64

	err := db.Table("oa_check_time").Where("DATE_FORMAT(check_date, '%Y%m')=?  and ding_id=?", peroid, dingId).Count(&total).Error
	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}

	limit, offset, pageInfo := ctx.QueryPageInfo(int(total))

	err = db.Table("oa_check_time").
		Where("DATE_FORMAT(check_date, '%Y%m')=?  and ding_id=?", peroid, dingId).
		Limit(limit).
		Offset(offset).
		Scan(&records).
		Error

	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}
	ctx.SuccessPage(http.StatusOK, records, pageInfo)
}

// AddOne add product
func (c *CheckTimeController) AddOne(ctx *httpctx.HTTPCtx, db *gorm.DB) {
	one := new(models.CheckTimeModel)
	ctx.DbAddOne(one)
}

func (c *CheckTimeController) AddOnes(ctx *httpctx.HTTPCtx, db *gorm.DB) {
	body, err := ctx.Req.Body().Bytes()
	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}

	var ones []models.CheckTimeModel
	//ones := make([]*models.CheckTimeModel, 0)
	err = json.Unmarshal(body, &ones)
	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}

	tx := db.Begin()
	for _, v := range ones {

		err = tx.Table("oa_check_time").Create(&v).Error
		if err != nil {
			break
		}
	}

	if err == nil {
		tx.Commit()
		ctx.Success(http.StatusOK, ones)
	} else {
		tx.Rollback()
		ctx.Error(http.StatusBadRequest, err.Error())
	}
}

// AddOne add product
func (c *CheckTimeController) UpdateOne(ctx *httpctx.HTTPCtx, db *gorm.DB) {
	one := new(models.CheckTimeModel)
	ctx.DbUpdateOne(one, "id")
}

// Delete 删除
func (c *CheckTimeController) Delete(ctx *httpctx.HTTPCtx, db *gorm.DB) {
	one := new(models.CheckTimeModel)
	ctx.DbDelete(one, "id")
}
