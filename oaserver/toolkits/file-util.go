package toolkits

import (
	"errors"
	"os"
)

func PathExists(path string) (bool, error) {
	_, err := os.Stat(path)
	if err == nil {
		return true, nil
	}
	if os.IsNotExist(err) {
		return false, nil
	}
	return false, err
}

func CreateOrGetPath(path string) error {
	if path == "" {
		return errors.New("path can not be null")
	}

	flag, err := PathExists(path)
	if err != nil {
		return err
	}

	if flag {
		return nil
	}

	err = os.Mkdir(path, os.ModePerm)
	return err
}
