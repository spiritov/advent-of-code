package main

import (
	"bufio"
	"fmt"
	"os"
	"strings"
)

const (
	Loss = iota * 3
	Draw
	Win
)

func getScore(first, second int) int {
	switch second - first {
	case 0:
		return Draw + second
	case 1, -2:
		return Win + second
	default:
		return Loss + second
	}
}

func getNewScore(first, outcome int) int {
	switch outcome {
	case Draw:
		return getScore(first, first)
	case Win:
		if first == 3 {
			return getScore(first, 1)
		} else {
			return getScore(first, first+1)
		}
	default:
		if first == 1 {
			return getScore(first, 3)
		} else {
			return getScore(first, first-1)
		}
	}
}

func main() {
	input, _ := os.Open("input.txt")
	read := bufio.NewScanner(input)
	var line string
	moves := map[string]int{
		"A": 1, //R
		"B": 2, //P
		"C": 3, //S
		"X": 1, //R
		"Y": 2, //P
		"Z": 3} //S
	outcomes := map[string]int{
		"X": 0, //Loss
		"Y": 3, //Draw
		"Z": 6} //Win

	var score, new_score int

	for read.Scan() {
		line = read.Text()
		first, second, _ := strings.Cut(line, " ")
		score += getScore(moves[first], moves[second])
		outcome := outcomes[second]
		new_score += getNewScore(moves[first], outcome)
	}
	fmt.Printf("part 1: %d\n", score)
	fmt.Printf("part 2: %d\n", new_score)
}
