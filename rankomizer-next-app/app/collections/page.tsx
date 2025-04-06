"use client";

import { useEffect, useState } from "react";
import { Card, CardContent } from "@/components/ui/card";
import Image from "next/image";

export default function CollectionsPage() {
  const [collections, setCollections] = useState([]);

  const fetchMovies = async () => {
    const res = await fetch("https://localhost:7135/api/collections");
    const data = await res.json();
    console.log("data", data);
    setCollections(data);
  };
  useEffect(() => {
    console.log("use effect triggered");
    fetchMovies();
  }, []);

  return (
    <div className="container mx-auto py-8">
      <h2 className="text-2xl font-bold mb-6">Popular Collections</h2>
      <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 md:gap-6">
        {collections &&
          collections.length > 0 &&
          collections.map((collection) => (
            <Card
              key={collection.id}
              className="overflow-hidden transition-all duration-200 hover:shadow-lg"
            >
              <div className="aspect-[2/3] relative">
                <Image
                  src={collection.imageUrl || "/placeholder.svg"}
                  alt={`${collection.name} poster`}
                  fill
                  className="object-cover"
                />
              </div>
              <CardContent className="p-3">
                <h3 className="font-medium text-sm md:text-base line-clamp-2">
                  {collection.name}
                </h3>
              </CardContent>
            </Card>
          ))}
      </div>
    </div>
  );
}
