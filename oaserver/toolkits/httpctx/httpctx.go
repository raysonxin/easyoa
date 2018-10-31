package httpctx

import (
	"encoding/json"
	"errors"
	"net/http"
	"strconv"
	"strings"

	"gopkg.in/macaron.v1"
)

// HTTPCtx custom http context
type HTTPCtx struct {
	*macaron.Context
}

// Context custom http context method
func Context() macaron.Handler {
	return func(ctx *macaron.Context) {
		ctx.Header().Add("X-Powered-By", "Golang/1.9.0")
		ctx.Header().Add("Access-Control-Allow-Origin", "*")
		myCtx := &HTTPCtx{
			Context: ctx,
		}
		ctx.Map(myCtx)
	}
}

// MapCustom inject custom objects
func MapCustom(m *macaron.Macaron, args ...interface{}) macaron.Handler {
	for _, v := range args {
		m.Map(v)
	}

	return func(c *macaron.Context) {

	}
}

// Error reponse error message,compatible for wechat mini app
func (ctx *HTTPCtx) Error(code int, err string) {
	if code == 0 {
		code = 400
	}
	ctx.JSON(code, map[string]interface{}{"code": code, "error": err})
}

// Success response success message,compatible for wechat mini app
func (ctx *HTTPCtx) Success(code int, v interface{}) {
	if code == 0 {
		code = http.StatusOK
	}
	ctx.JSON(code, map[string]interface{}{"code": code, "data": v})
}

// CheckFormData check client post data
func (ctx *HTTPCtx) CheckFormData(v ...interface{}) error {
	body, err := ctx.Req.Body().Bytes()
	if err != nil {
		return err
	}

	for _, obj := range v {
		err = json.Unmarshal(body, obj)
		if err != nil {
			return err
		}
	}
	return nil
}

// GetDelIDs get to delete ids
func (ctx *HTTPCtx) GetDelIDs() ([]string, error) {
	ids := ctx.Params(":ids")
	// 删除数据，id不能为空
	if ids == "" {
		return nil, errors.New("the :ids params does not exists")
	}
	// 验证ids是否是逗号隔开字符串
	idsStrs := strings.Split(ids, ",")
	for _, v := range idsStrs {
		_, err := strconv.Atoi(v)
		if err != nil {
			return nil, errors.New("the :ids params's format is invalid")
		}
	}
	return idsStrs, nil
}
