package models

// EmployeeDingModel dingding id
type EmployeeDingModel struct {
	Id     int    `gorm:"column:id;primary_key"` //主键编号
	EmpId  int    `gorm:"column:emp_id"`         //
	DingId string `gorm:"column:ding_id"`        //
}

// TableName get table name
func (t EmployeeDingModel) TableName() string {
	return "oa_employee_ding"
}
