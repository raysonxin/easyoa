package toolkits

// HttpConfig  config struct for httpsever
type HTTPConfig struct {
	Address string `toml:"address"` //http server host address
	Port    int    `toml:"port"`    //http server listen port
}

// Init 初始化http配置
func (cfg *HTTPConfig) Init() {
	if cfg.Address == "" {
		cfg.Address = "0.0.0.0"
	}

	if cfg.Port == 0 {
		cfg.Port = 8973
	}
}
