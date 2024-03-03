"use client";

import React from "react";
import { useRouter } from "next/navigation";
import { Button } from "@/components/ui/button";

/**
 * Client-side component used in the MovieDetail scene to display a link to go back to the search page.
 */
const BackToSearchLink = () => {
  const { back } = useRouter();
  return (
    <Button
      variant="link"
      className="text-gray-300 mt-4 ml-2 md:absolute md:top-5 md:left-5"
      onClick={back}
    >
      Back to search
    </Button>
  );
};

export default BackToSearchLink;
