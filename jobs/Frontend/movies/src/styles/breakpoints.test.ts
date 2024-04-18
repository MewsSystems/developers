import { media, device, DeviceSizes } from "./breakpoints";

describe("media", () => {
  it.each([
    ["xs" as DeviceSizes],
    ["sm" as DeviceSizes],
    ["md" as DeviceSizes],
    ["lg" as DeviceSizes],
    ["xl" as DeviceSizes],
    ["xxl" as DeviceSizes],
  ])("media %s", (size: DeviceSizes) => {
    const result = media[size]`display: flex;`;
    const resultText = result.join("");

    expect(resultText).toContain(device[size]);
    expect(resultText).toContain("display: flex;");
  });
});
