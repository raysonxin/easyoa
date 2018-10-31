package models

// EmployeeModel employee model
type EmployeeModel struct {
	Id     int    `gorm:"column:id;primary_key"`
	Name   string `gorm:"column:name"`
	Job    int    `gorm:"column:job"`
	DingId string `gorm:"column:ding_id"`
}

// TableName 表名
func (m EmployeeModel) TableName() string {
	return "oa_employee"
}
