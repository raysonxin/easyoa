package main

import (
	"fmt"
	"runtime"

	"github.com/raysonxin/easyoa/oaserver/httpserver"
)

func main() {
	runtime.GOMAXPROCS(runtime.NumCPU())

	svr := httpserver.New()
	// config := &service.Config{
	// 	Name:        "http-server",
	// 	DisplayName: "http-server",
	// 	Description: "http-server",
	// }

	// p,err:=
	svr.Start()

	fmt.Print("Hello World!")
}
