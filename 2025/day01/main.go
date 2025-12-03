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

func main() {
	lines := strings.Split(input, "\n")
	for i, line := range lines {
		if line[:1] == "L" {
			lines[i] = "-" + line[1:]
		} else { // assume "R"
			lines[i] = "+" + line[1:]
		}
	}

	fmt.Printf("part 1: %d\n", part(1, lines))
	fmt.Printf("part 2: %d\n", part(2, lines))
}

func part(part int, lines []string) int {
	dial := 50
	zeroTouches := 0

	for _, rotation := range lines {
		turn, err := strconv.Atoi(rotation)
		if err != nil {
			log.Fatal(err)
		}

		if part == 2 {
			remTurn := turn % 100
			if turn > 0 {
				zeroTouches += turn / 100
				if dial != 0 && dial+remTurn > 100 {
					zeroTouches++
				}
			} else {
				zeroTouches -= turn / 100
				if dial != 0 && dial+remTurn < 0 {
					zeroTouches++
				}
			}
		}

		// account for negatives when using modulus
		// ex. -18 % 100 will return -18, but we want 82
		dial = (((dial + turn) % 100) + 100) % 100

		if dial == 0 {
			zeroTouches++
		}
	}

	return zeroTouches
}
