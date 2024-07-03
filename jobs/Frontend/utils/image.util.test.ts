import { getImageUrl } from "./image.util";

// Simple unit test for demonstration purposes
describe("image.util.ts", () => {
  it("prepend / to url if it does not contain one", () => {
    const imageUrl = "fiVW06jE7z9YnO4trhaMEdclSiC.jpg";

    expect(getImageUrl(500, imageUrl)).toContain(
      "/fiVW06jE7z9YnO4trhaMEdclSiC.jpg",
    );
  });

  it("preserve url if contains a starting /", () => {
    const imageUrl = "/fiVW06jE7z9YnO4trhaMEdclSiC.jpg";

    expect(getImageUrl(500, imageUrl)).not.toContain("//");
  });
});
