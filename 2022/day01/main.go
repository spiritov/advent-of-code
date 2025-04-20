package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
)

func main() {
	input, _ := os.Open("input.txt")
	read := bufio.NewScanner(input)
	var line string
	maxes := make([]int, 3)
	cal := 0

	for read.Scan() {
		line = read.Text()
		if len(line) == 0 {
			for i, max := range maxes {
				if cal > max {
					maxes[i] = cal
					cal = max
				}
			}
			cal = 0
		} else {
			food_cal, _ := strconv.Atoi(line)
			cal += food_cal
		}
	}

	fmt.Printf("part 1: %d\n", maxes[0])
	fmt.Printf("part 2: %d\n", maxes[0]+maxes[1]+maxes[2])
}
