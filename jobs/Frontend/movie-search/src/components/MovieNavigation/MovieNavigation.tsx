"use client";
import { useRouter } from "next/navigation";
import React from "react";
import {
  BackButton,
  NavigationHeader,
} from "./MovieNavigationStyledComponents";

interface MovieNavigationProps {
  isFromSearch: boolean;
}

export default function MovieNavigation({
  isFromSearch,
}: MovieNavigationProps) {
  const router = useRouter();

  if (isFromSearch) {
    return (
      <NavigationHeader>
        <BackButton onClick={() => router.back()}>Back To Search</BackButton>
      </NavigationHeader>
    );
  }

  return (
    <NavigationHeader>
      <BackButton onClick={() => router.push("/")}>Home</BackButton>;
    </NavigationHeader>
  );
}
