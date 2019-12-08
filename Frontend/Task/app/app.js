import { endpoint, interval } from "./config";

import { init } from "./sam/Actions";

export function run() {
    init(endpoint, interval);
}
