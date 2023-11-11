export default function TestPage() {
  const test = fetch("https://jsonplaceholder.typicode.com/posts").then(() => {
    throw new Error("error server");
  });

  return <h1>TEST PAGE</h1>;
}
