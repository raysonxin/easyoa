/*
 * @Author: raysonxin
 * @Date: 2018-03-06 23:43:13
 * @Last Modified by:   raysonxin
 * @Last Modified time: 2018-03-06 23:43:13
 */
package mysql

import "time"

// Config 定义mysql数据库配置配置信息结构
type Config struct {
	Debug        bool          `toml:"debug"`          // 是否调试模式
	Address      string        `toml:"address"`        // 数据库连接地址
	Port         int           `toml:"port"`           // 数据库端口
	MaxIdleConns int           `toml:"max_idle_conns"` // 连接池最大连接数
	MaxOpenConns int           `toml:"max_open_conns"` // 默认打开连接数
	User         string        `toml:"user"`           // 数据库用户名
	Passwd       string        `toml:"passwd"`         // 数据库密码
	DbName       string        `toml:"db_name"`        // 数据库名
	Prefix       string        `toml:"prefix"`         // 数据库表前缀
	PingInterval time.Duration `toml:"ping_interval"`  // 定时保活时间间隔
}

// Init 初始化数据库配置
func (cfg *Config) Init() {
	if cfg.Address == "" {
		cfg.Address = "127.0.0.1"
	}
	if cfg.Port == 0 {
		cfg.Port = 3306
	}
	if cfg.MaxIdleConns == 0 {
		cfg.MaxIdleConns = 64
	}
	if cfg.MaxOpenConns == 0 {
		cfg.MaxOpenConns = 16
	}
	if cfg.User == "" {
		cfg.User = "root"
	}
	if cfg.PingInterval == 0 {
		duration, _ := time.ParseDuration("10s")
		cfg.PingInterval = time.Duration(duration)
	}
}
