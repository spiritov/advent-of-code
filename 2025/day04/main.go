package main

import (
	_ "embed"
	"fmt"
	"slices"
	"strings"
)

type pos struct {
	x, y int
}

func addPositions(a pos, b pos) pos {
	return pos{x: a.x + b.x, y: a.y + b.y}
}

func areEqualPositions(a pos, b pos) bool {
	return a.x == b.x && a.y == b.y
}

//go:embed input.txt
var input string
var positions []pos
var deletables []pos
var directions = []pos{
	{-1, -1}, {-1, 0}, {-1, 1},
	{0, -1}, {0, 1},
	{1, -1}, {1, 0}, {1, 1}}

func main() {
	lines := strings.Split(input, "\n")
	positions = getPaperPositions(lines)

	fmt.Printf("part 1: %d\n", part(1))
	fmt.Printf("part 2: %d\n", part(2))
}

func part(part int) int {
	sum := 0

	if part == 1 {
		sum = getSumOfRemovables(1)
	} else {
		for {
			loopSum := getSumOfRemovables(2)
			sum += loopSum
			if (loopSum) == 0 {
				break
			}

			for _, deletable := range deletables {
				positions = slices.DeleteFunc(positions, func(p pos) bool {
					return areEqualPositions(p, deletable)
				})
			}
		}
	}

	return sum
}

func getPaperPositions(lines []string) []pos {
	var positions []pos
	for i, line := range lines {
		for j, obj := range line {
			if obj == '@' {
				positions = append(positions, pos{x: i, y: j})
			}
		}
	}
	return positions
}

func getSumOfRemovables(part int) int {
	sum := 0

	for _, position := range positions {
		neighbors := 0

		for _, direction := range directions {
			if slices.Contains(positions, addPositions(position, direction)) {
				neighbors++
			}
		}

		if neighbors < 4 {
			sum++
			if part == 2 {
				deletables = append(deletables, position)
			}
		}
	}

	return sum
}
