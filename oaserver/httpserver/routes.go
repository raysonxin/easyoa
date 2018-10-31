package httpserver

import (
	"github.com/raysonxin/easyoa/oaserver/httpserver/v1"
)

// RegisterRouter register http router
func (svr *HTTPServer) RegisterRouter() {
	v1.RegisterRoutes(svr.ctx)
}
