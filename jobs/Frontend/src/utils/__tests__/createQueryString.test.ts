import createQueryString from "../createQueryString";

describe("createQueryString()", () => {
  it("returns query string", () => {
    expect(createQueryString({ query: "Venom", page: 2 })).toBe(
      "?query=Venom&page=2"
    );
  });

  it("returns query string only for non null values", () => {
    expect(createQueryString({ query: null, page: 2 })).toBe("?page=2");
  });

  it("returns query string only for non empty values", () => {
    expect(createQueryString({ query: "", page: 2 })).toBe("?page=2");
  });

  it("returns empty string for nothing provided", () => {
    expect(createQueryString({ query: "", page: null })).toBe("");
  });
});
