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
	input, _ := strings.CutSuffix(input, "\n")
	lines := strings.Split(input, ",")

	fmt.Printf("part 1: %d\n", part(1, lines))
	fmt.Printf("part 2: %d\n", part(2, lines))
}

func part(part int, lines []string) int {
	sum := 0

	for _, line := range lines {
		invalidRange := strings.Split(line, "-")

		first, err := strconv.Atoi(invalidRange[0])
		if err != nil {
			log.Fatal(err)
		}
		last, err := strconv.Atoi(invalidRange[1])
		if err != nil {
			log.Fatal(err)
		}

		for i := first; i <= last; i++ {
			id := strconv.Itoa(i)
			idLen := len(id)

			if part == 1 && idLen%2 != 0 {
				continue
			}

			if part == 1 && id[:idLen/2] == id[idLen/2:] {
				sum += i
			}

			if part == 2 && idLen > 1 {
				divisors := getDivisors(idLen)
				for _, divisor := range divisors {
					if isInvalidID(id, divisor) {
						sum += i
						break
					}
				}
			}
		}
	}

	return sum
}

func getDivisors(n int) []int {
	divisors := []int{n}
	for i := 2; i <= n/2; i++ {
		if n%i == 0 {
			divisors = append(divisors, i)
		}
	}
	return divisors
}

func isInvalidID(id string, divisor int) bool {
	idParts := []string{}
	partLen := len(id) / divisor

	for i := range divisor {
		partI := i * partLen
		idParts = append(idParts, id[partI:partI+partLen])
	}

	for i := 1; i < len(idParts); i++ {
		if idParts[i-1] != idParts[i] {
			return false
		}
	}

	return true
}
