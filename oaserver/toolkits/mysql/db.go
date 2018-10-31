/*
 * @Author: raysonxin
 * @Date: 2017-10-20 16:02:22
 * @Last Modified by: mikey.zhaopeng
 * @Last Modified time: 2017-10-20 16:16:07
 */

package mysql

import (
	"errors"
	"fmt"
	"time"

	_ "github.com/go-sql-driver/mysql"
	"github.com/jinzhu/gorm"
	"github.com/raysonxin/easyoa/oaserver/toolkits/logs"
)

// DB mysql database orm object
type DB struct {
	db     *gorm.DB
	config *Config
}

// GetMySQLDB get mysql database orm object
func (mysql *DB) GetMySQLDB() *gorm.DB {
	if mysql.db == nil {
		//logger.Errorln(logger.ERROR_REDIS_GET_CLIENT, "Mysql连接为nil")
		return nil
	}
	return mysql.db
}

// Close close org object
func (mysql *DB) Close() error {
	if mysql.db != nil {
		return mysql.db.Close()
	}
	return nil
}

// New create a new mysql database orm object
func New(config *Config) (*DB, error) {

	logger := logs.NewLogger("db tool", "logs/log")

	if config == nil {
		return nil, errors.New("params config can't be nil.\r")
	}
	if config.Address == "" {
		return nil, errors.New("the host for mysql address can't be nil.\r")
	}
	logger.Info("INFO_MySQL_CONN", "connecting to mysql...")

	// build the connecting string
	connStr := fmt.Sprintf("%s:%s@(%s:%d)/%s?charset=utf8&parseTime=True&loc=UTC",
		config.User,
		config.Passwd,
		config.Address,
		config.Port,
		config.DbName)

	// open database
	db, err := gorm.Open("mysql", connStr)
	// set prefix for tables
	gorm.DefaultTableNameHandler = func(db *gorm.DB, defaultTableName string) string {
		return config.Prefix + defaultTableName
	}
	// 禁用表名多元化
	db.SingularTable(true)
	// wheather debug mode or not
	if config.Debug {
		// debug
		db = db.Debug()
	}

	// MaxIdleConns
	db.DB().SetMaxIdleConns(config.MaxIdleConns)
	// MaxOpenConns
	db.DB().SetMaxOpenConns(config.MaxOpenConns)

	// keepalive
	go func() {
		for {
			// ping
			err = db.DB().Ping()
			if err != nil {
				logger.Error("connect db error", err.Error())
			}
			// interval
			time.Sleep(time.Duration(config.PingInterval))
		}
	}()
	mysqlDB := &DB{db: db, config: config}
	logger.Info("INFO_MySQL_CONN", "mysql database connected.")
	return mysqlDB, err
}
