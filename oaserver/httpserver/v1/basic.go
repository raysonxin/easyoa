package v1

import (
	"fmt"
	"io/ioutil"
	"net/http"

	"github.com/jinzhu/gorm"
	"github.com/raysonxin/easyoa/oaserver/toolkits/httpctx"
)

// BasicController basic api
type BasicController struct {
}

// GetUserInfo get wechat user info
func (c *BasicController) GetUserInfo(ctx *httpctx.HTTPCtx, db *gorm.DB) {
	// shopId := ctx.Query("shop")
	// if shopId == "" {
	// 	ctx.Error(http.StatusBadRequest, "shop can not be null")
	// 	return
	// }

	code := ctx.Query("code")
	if code == "" {
		ctx.Error(http.StatusBadRequest, "code can not be null")
		return
	}

	// var shop models.ShopModel
	// err := db.Table("shop").Where("id=?", shopId).First(&shop).Error
	// if err != nil {
	// 	ctx.Error(http.StatusBadRequest, err.Error())
	// 	return
	// }

	client := &http.Client{}
	url := fmt.Sprintf("https://api.weixin.qq.com/sns/jscode2session?appid=%s&secret=%s&js_code=%s&grant_type=authorization_code", "app-id", "secret", code)
	request, err := http.NewRequest("GET", url, nil)
	if err != nil {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}

	request.Header.Set("content-type", "application/json")
	response, err := client.Do(request)

	if response.StatusCode != 200 {
		ctx.Error(http.StatusBadRequest, err.Error())
		return
	}

	body, _ := ioutil.ReadAll(response.Body)
	// var result models.WechatResponse
	// json.Unmarshal(body, &result)
	ctx.Success(http.StatusOK, string(body))
}
