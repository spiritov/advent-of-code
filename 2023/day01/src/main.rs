use std::fs::read_to_string;

struct Digit {
    index: usize,
    value: u32,
}

fn main() {
    let file = "input.txt";
    let input: Vec<String> = read_to_string(file)
        .unwrap()
        .lines()
        .map(str::to_string)
        .collect();

    let str_digits = [
        "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
    ];
    let mut found_digits: Vec<Digit> = Vec::new();
    let mut sum = 0;
    let mut sum2 = 0;

    for ln in input.iter() {
        for (index, c) in ln.chars().enumerate() {
            if c.is_digit(10) {
                found_digits.push(Digit {
                    index,
                    value: char::to_digit(c, 10).unwrap(),
                })
            }
        }
        found_digits.sort_by_key(|d| d.index);
        sum += (found_digits[0].value * 10) + found_digits.last().unwrap().value;

        for (i, str_digit) in str_digits.iter().enumerate() {
            let found_indexes: Vec<usize> = ln.match_indices(str_digit).map(|(i, _)| i).collect();
            for index in found_indexes {
                found_digits.push(Digit {
                    index,
                    value: (i as u32) + 1,
                })
            }
        }
        found_digits.sort_by_key(|d| d.index);
        sum2 += (found_digits[0].value * 10) + found_digits.last().unwrap().value;

        found_digits.clear();
    }

    println!("Part 1: {sum}");
    println!("Part 2: {sum2}");
}
