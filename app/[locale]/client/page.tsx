"use client";

export default function ClientPage() {
  return (
    <div>
      <h1>Client Page</h1>
      <button
        onClick={() => {
          throw new Error("client error");
        }}
      >
        Throw new error
      </button>
    </div>
  );
}
