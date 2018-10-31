package models

// UserModel User model
type UserModel struct {
	Id   int    `gorm:"column:id;primary_key"`
	Name string `gorm:"column:name"`
	Pwd  string `gorm:"column:pwd"`
}

// TableName 返回表名
func (m UserModel) TableName() string {
	return "oa_user"
}
