"use client";

import { useEffect, useState } from "react";
import { Card, CardContent } from "@/components/ui/card";
import Image from "next/image";

export default function CollectionsPage() {
  const [movies, setMovies] = useState({ items: [] });

  const fetchMovies = async () => {
    const res = await fetch(
      "https://localhost:7135/Collections/collections/01960866-9454-7f11-a726-3a1efcc02ab5"
    );
    const data = await res.json();
    console.log("data", data);
    setMovies(data);
  };
  useEffect(() => {
    console.log("use effect triggered");
    fetchMovies();
  }, []);

  return (
    <div className="container mx-auto py-8">
      <h2 className="text-2xl font-bold mb-6">Popular Movies</h2>
      <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 md:gap-6">
        {movies &&
          movies.items.map((movie) => (
            <Card
              key={movie.id}
              className="overflow-hidden transition-all duration-200 hover:shadow-lg"
            >
              <div className="aspect-[2/3] relative">
                <Image
                  src={movie.imageUrl || "/placeholder.svg"}
                  alt={`${movie.name} poster`}
                  fill
                  className="object-cover"
                />
              </div>
              <CardContent className="p-3">
                <h3 className="font-medium text-sm md:text-base line-clamp-1">
                  {movie.name} (
                  {new Date(movie.details.sourceJson.releaseDate).getFullYear()}
                  )
                </h3>
              </CardContent>
            </Card>
          ))}
      </div>
    </div>
  );
}
