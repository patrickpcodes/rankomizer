"use client";

import React, { useEffect, useState } from "react";
import { MiniCards } from "./GauntletRanking";
import { GauntletDuel } from "./GauntletDuel";
import { Star } from "lucide-react";

type ItemType = "Movie" | "Song" | "Painting";

export type DuelDto = {
  duelId: string;
  optionA: RosterItemDto;
  optionB: RosterItemDto;
};
export type MiniItemDto = {
  id: string;
  name: string;
  imageUrl: string;
};

export type CompletedDuelDto = {
  duelId: string;
  item1: MiniItemDto;
  item2: MiniItemDto;
  winnerId: string;
};

// Extend DuelResponseDto to include the completed duels
export type DuelResponseDto = {
  duel: DuelDto | null;
  roster: RosterItemDto[];
  completedDuels: CompletedDuelDto[];
};
export type RosterItemDto = {
  id: string;
  name: string;
  imageUrl: string;
  itemType: ItemType;
  status: string;
  wins: number;
  losses: number;
  score: number;
  details?: any; // skip parsing for now
};

type SubmitDuelRequest = {
  duelId: string;
  winnerRosterItemId: string;
};

interface GauntletDuelProps {
  gauntletId: string;
}

export default function Gauntlet({ gauntletId }: GauntletDuelProps) {
  const [duel, setDuel] = useState<DuelDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [finished, setFinished] = useState(false);
  const [order, setOrder] = useState(false);
  const [roster, setRoster] = useState<RosterItemDto[]>([]);
  const [completedDuels, setCompletedDuels] = useState<CompletedDuelDto[]>([]);

  useEffect(() => {
    fetchNextDuel();
  }, []);

  async function fetchNextDuel() {
    setLoading(true);
    const res = await fetch(
      `${process.env.NEXT_PUBLIC_API_BASE_URL}/api/gauntlet/${gauntletId}/start`,
      {
        method: "POST",
        credentials: "include",
      }
    );

    const data: DuelResponseDto = await res.json();
    setDuel(data.duel);
    setRoster(data.roster);
    setCompletedDuels(data.completedDuels);
    if (!data.duel) {
      setFinished(true);
    } else {
      setOrder(Math.random() < 0.5);
    }
    setSubmitting(false);
    setLoading(false);
  }

  async function handlePick(winnerId: string) {
    if (!duel) return;

    setSubmitting(true);
    const res = await fetch(
      `${process.env.NEXT_PUBLIC_API_BASE_URL}/api/gauntlet/duel/submit`,
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          duelId: duel.duelId,
          winnerRosterItemId: winnerId,
        } as SubmitDuelRequest),
        credentials: "include",
      }
    );

    const data: DuelResponseDto = await res.json();
    setDuel(data.duel);
    setRoster(data.roster);
    setCompletedDuels(data.completedDuels);

    if (!data.duel) {
      setFinished(true);
    } else {
      setOrder(Math.random() < 0.5);
    }
    setSubmitting(false);

    // if (!data) {
    //   setDuel(null);
    //   setRoster([]);
    //   setFinished(true);
    // } else {
    //   setDuel(data);
    // }
  }

  if (loading) return <p>Loading duel...</p>;
  //   if (finished) return <p>🔥 Gauntlet complete!</p>;
  //   if (!duel) return <p>Nothing to show</p>;

  return (
    <div className="flex flex-row justify-center gap-6">
      <div className="flex flex-col items-center ">
        {duel && (
          <GauntletDuel
            duel={duel}
            handlePick={handlePick}
            submitting={submitting}
          />
        )}

        {/* {submitting && <p>Submitting choice...</p>} */}
        {completedDuels && completedDuels.length > 0 && (
          <div className="max-w-2xl mx-auto border rounded-lg p-4 shadow-sm">
            <h2 className="text-lg font-semibold mb-3">
              Battle History : {completedDuels.length} Completed Duels
            </h2>
            <div className="space-y-3">
              {completedDuels.map((battle, index) => (
                <div key={battle.duelId} className="flex items-center text-xs">
                  <div
                    className={`flex items-center w-1/3 ${
                      battle.winnerId === battle.item1.id
                        ? "font-medium"
                        : "opacity-70"
                    }`}
                  >
                    <img
                      src={battle.item1.imageUrl || "/placeholder.svg"}
                      alt={battle.item1.name}
                      className="w-8 h-12 object-cover rounded"
                    />
                    <div className="ml-1 overflow-hidden">
                      <p className="truncate">{battle.item1.name}</p>
                      {battle.winnerId === battle.item1.id && (
                        <div className="flex items-center text-green-600">
                          <Star className="w-3 h-3 mr-1 fill-current" />
                          <span className="text-[10px]">Winner</span>
                        </div>
                      )}
                    </div>
                  </div>

                  <div className="w-1/3 text-[10px] text-gray-500 text-center">
                    Battle #{completedDuels.length - index}
                  </div>

                  <div
                    className={`flex items-center justify-end w-1/3 ${
                      battle.winnerId === battle.item2.id
                        ? "font-medium"
                        : "opacity-70"
                    }`}
                  >
                    <div className="mr-1 text-right overflow-hidden">
                      <p className="truncate">{battle.item2.name}</p>
                      {battle.winnerId === battle.item2.id && (
                        <div className="flex items-center justify-end text-green-600">
                          <Star className="w-3 h-3 mr-1 fill-current" />
                          <span className="text-[10px]">Winner</span>
                        </div>
                      )}
                    </div>
                    <img
                      src={battle.item2.imageUrl || "/placeholder.svg"}
                      alt={battle.item2.name}
                      className="w-8 h-12 object-cover rounded"
                    />
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}
      </div>
      <div className="overflow-y-auto">
        {roster && roster.length > 0 && <MiniCards items={roster} />}
      </div>
    </div>
  );
}
