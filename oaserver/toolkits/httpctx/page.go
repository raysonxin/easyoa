package httpctx

import (
	"math"
	"net/http"
)

// PageInfo used to data page
type PageInfo struct {
	Index int `json:"Index"` // current page index
	Total int `json:"Total"` // total pages
	Rows  int `json:"Rows"`  // rows count each page
}

// QueryPageInfo build page info from http context
func (ctx *HTTPCtx) QueryPageInfo(recordCount int) (rows, offset int, pageInfo *PageInfo) {
	index := ctx.QueryInt("index") // query page index
	rows = ctx.QueryInt("rows")    // query user set rows
	isAll := ctx.QueryBool("all")  // wheather get all or not
	if index == 0 {
		index = 1
	}

	if isAll == true {
		rows = 1<<31 - 1
	} else if rows == 0 {
		rows = 10
	}

	pageInfo = new(PageInfo)
	// if recordCount is 0,indicates that we can get nothing by user's request
	if recordCount == 0 {
		return 0, 0, pageInfo
	}
	// 保存页码信息
	pageInfo.Index = index
	pageInfo.Rows = rows
	pageInfo.Total = int(math.Ceil(float64(recordCount) / float64(rows)))

	offset = (index - 1) * rows // 分页sql使用
	return
}

// SuccessPage response success message,compatible for wechat mini app
func (ctx *HTTPCtx) SuccessPage(code int, v interface{}, p *PageInfo) {
	if code == 0 {
		code = http.StatusOK
	}
	ctx.JSON(code, map[string]interface{}{"code": code, "data": v, "page": p})
}
