"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { Card, CardContent } from "@/components/ui/card";
import Image from "next/image";
import Modal from "@/components/BasicModal"; // Assume you have a Modal component or create one

export default function CollectionsPage() {
  const [collections, setCollections] = useState([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedCollectionId, setSelectedCollectionId] = useState<
    string | null
  >(null);
  const [gauntletName, setGauntletName] = useState("");
  const router = useRouter();

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

  const handleCollectionClick = (collectionId: string) => {
    setSelectedCollectionId(collectionId);
    setIsModalOpen(true);
  };

  const handleModalSubmit = async () => {
    if (!selectedCollectionId || !gauntletName) return;

    try {
      const response = await fetch(
        "https://localhost:7135/api/gauntlet/create",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            collectionId: selectedCollectionId,
            gauntletName,
          }),
        }
      );

      if (!response.ok) {
        throw new Error("Failed to create gauntlet");
      }

      const data = await response.json();
      const { gauntletId } = data;

      // Navigate to the gauntlet page
      router.push(`/gauntlet/${gauntletId}`);
    } catch (error) {
      console.error("Error creating gauntlet:", error);
    } finally {
      setIsModalOpen(false);
      setGauntletName("");
    }
  };

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
              onClick={() => handleCollectionClick(collection.id)}
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

      {isModalOpen && (
        <Modal onClose={() => setIsModalOpen(false)}>
          <div className="p-4">
            <h2 className="text-lg font-bold mb-4">Enter Gauntlet Name</h2>
            <input
              type="text"
              value={gauntletName}
              onChange={(e) => setGauntletName(e.target.value)}
              className="w-full p-2 border rounded mb-4"
              placeholder="Enter name"
            />
            <button
              onClick={handleModalSubmit}
              className="bg-blue-500 text-white px-4 py-2 rounded"
            >
              Submit
            </button>
          </div>
        </Modal>
      )}
    </div>
  );
}
