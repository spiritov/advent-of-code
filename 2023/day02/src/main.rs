use std::fs::read_to_string;

fn main() {
    let input: Vec<String> = read_to_string("input.txt")
        .unwrap()
        .lines()
        .map(|ln| ln.replace([' '], ""))
        .collect();

    let max_cubes = [12, 13, 14];
    let colors = ["red", "green", "blue"];
    let mut sum = 0;
    let mut sum_power = 0;
    for (ln_i, ln) in input.iter().enumerate() {
        let mut game_max_cubes = [0, 0, 0];
        let game = ln[(ln.find(':').unwrap() + 1)..].split(';');
        for set in game {
            for pull in set.split(',') {
                for (c_i, s) in colors.iter().enumerate() {
                    if pull.contains(s) {
                        let num_cubes = pull.replace(s, "").parse::<i32>().unwrap();
                        if num_cubes > game_max_cubes[c_i] {
                            game_max_cubes[c_i] = num_cubes;
                        }
                    }
                }
            }
        }

        if !(game_max_cubes[0] > max_cubes[0]
            || game_max_cubes[1] > max_cubes[1]
            || game_max_cubes[2] > max_cubes[2])
        {
            sum += ln_i + 1;
        }
        sum_power += game_max_cubes[0] * game_max_cubes[1] * game_max_cubes[2];
    }
    println!("Part 1: {sum}");
    println!("Part 2: {sum_power}");
}
