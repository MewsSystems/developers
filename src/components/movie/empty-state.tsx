import React from "react";

interface EmptyStateProps {
    message?: string;
}

export function EmptyState({ message = "No Results Found" }: EmptyStateProps) {
    return (
        <div className="col-span-full flex h-[calc(100vh-200px)] items-center justify-center">
            <p className="text-lg text-gray-500">{message}</p>
        </div>
    );
} 