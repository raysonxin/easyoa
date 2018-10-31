package v1

import (
	"github.com/raysonxin/easyoa/oaserver/toolkits/httpctx"
	macaron "gopkg.in/macaron.v1"
)

// RegisterRoutes register routers
func RegisterRoutes(m *macaron.Macaron) {

	//basic := new(BasicController)
	emp := new(EmployeeController)
	proj := new(ProjectController)
	staff := new(ProjectStaffController)
	check := new(CheckTimeController)

	m.Group("/v1", func() {
		m.Options("/*", func(ctx *httpctx.HTTPCtx) {
			ctx.Header().Add("Access-Control-Allow-Methods", "POST,GET,OPTIONS,PUT,DELETE")
			ctx.Header().Add("Access-Control-Allow-Header", "Qsc-Token,Content-TYpe")
		})

		m.Group("/oa", func() {
			m.Group("/employee", func() {
				m.Get("", emp.List)
				m.Post("", emp.AddOne)
				m.Put("/:id", emp.UpdateOne)
				m.Delete("/:ids", emp.Delete)
			})

			m.Group("/project", func() {
				m.Get("", proj.List)
				m.Post("", proj.AddOne)
				m.Put("/:id", proj.UpdateOne)
				m.Delete("/:ids", proj.Delete)
			})

			m.Group("/projstaff", func() {
				m.Get("", staff.List)
				m.Post("", staff.AddOne)
				m.Put("/:id", staff.UpdateOne)
				m.Delete("/:ids", staff.Delete)
			})

			m.Group("/checktime", func() {
				m.Get("", check.List)
				m.Post("/ones", check.AddOnes)
				m.Put("/:id", check.UpdateOne)
				m.Delete("/:ids", check.Delete)
			})

		})
	})
}
