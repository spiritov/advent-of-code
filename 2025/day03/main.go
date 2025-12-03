package main

import (
	_ "embed"
	"fmt"
	"log"
	"strconv"
	"strings"
)

//go:embed input.txt
var input string
var digits []rune

func main() {
	lines := strings.Split(input, "\n")

	fmt.Printf("part 1: %d\n", part(1, lines))
	fmt.Printf("part 2: %d\n", part(2, lines))
}

func part(part int, lines []string) int {
	total := 0

	for _, bank := range lines {
		minIndex := 0
		initJoltage(part)
		for i := range digits {
			for j, battery := range bank {
				if battery > digits[i] && j >= minIndex && j <= len(bank)-len(digits)+i {
					digits[i] = battery
					minIndex = j + 1
				}
			}
		}

		maxJoltage, err := strconv.Atoi(string(digits))
		if err != nil {
			log.Fatal(err)
		}
		total += maxJoltage
	}

	return total
}

func initJoltage(part int) {
	digits = []rune{}
	n := 0
	if part == 1 {
		n = 2
	} else {
		n = 12
	}
	for range n {
		digits = append(digits, '1')
	}
}
