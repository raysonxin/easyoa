package toolkits

import (
	"errors"
	"fmt"
	"os"
	"path/filepath"

	"github.com/naoina/toml"
	"github.com/raysonxin/easyoa/oaserver/toolkits/mysql"
)

// ServiceCfg 配置文件读取助手
type ServiceCfg struct {
	DebugMode bool          `toml:"debug_mode"` // 是否调试模式
	Mysql     *mysql.Config `toml:"mysql"`      // mysql 配置
	HTTP      *HTTPConfig   `toml:"http"`       // http configuration
}

// NewServiceCfg 读取配置文件内容
func NewServiceCfg(path string) (*ServiceCfg, error) {
	if path == "" {
		path = fmt.Sprintf("%sconf%sconf.toml", GetRootDir(), string(os.PathSeparator))
		fmt.Println(path)
	}
	f, err := os.Open(path)
	if err != nil {
		return nil, err
	}
	defer f.Close()

	cfg := new(ServiceCfg)
	if err := toml.NewDecoder(f).Decode(cfg); err != nil {
		return nil, err
	}

	if cfg.HTTP == nil {
		return nil, errors.New("http config is empty")
	}
	cfg.HTTP.Init()

	if cfg.Mysql == nil {
		return nil, errors.New("mysql config is empty")
	}
	cfg.Mysql.Init()

	return cfg, nil
}

// GetRootDir 获取程序跟目录,返回值包含'/'
func GetRootDir() string {
	// 文件不存在获取执行路径
	file, err := filepath.Abs(filepath.Dir(os.Args[0]))
	if err != nil {
		file = fmt.Sprintf(".%s", string(os.PathSeparator))
	} else {
		file = fmt.Sprintf("%s%s", file, string(os.PathSeparator))
	}
	return file
}
