"use client";

import React from "react";
import { useRouter } from "next/navigation";
import { Button } from "@/components/ui/button";

const BackToSearchLink = () => {
  const { back } = useRouter();
  return (
    <Button
      variant="link"
      className="text-gray-300 absolute top-5 left-5"
      onClick={back}
    >
      Back to search
    </Button>
  );
};

export default BackToSearchLink;
