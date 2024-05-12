"use client";
import { useRouter } from "next/navigation";
import React from "react";

interface MovieNavigationProps {
  isFromSearch: boolean;
}

export default function MovieNavigation({
  isFromSearch,
}: MovieNavigationProps) {
  const router = useRouter();

  if (isFromSearch) {
    return <button onClick={() => router.back()}>Back To Search</button>;
  }

  return <button onClick={() => router.push("/")}>Home</button>;
}
