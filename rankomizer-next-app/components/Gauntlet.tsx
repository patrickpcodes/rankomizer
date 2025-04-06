"use client";

import React, { useEffect, useState } from "react";
import { MiniCards } from "./GauntletRanking";
import { GauntletDuel } from "./GauntletDuel";

type ItemType = "Movie" | "Song" | "Painting";

export type DuelDto = {
  duelId: string;
  optionA: RosterItemDto;
  optionB: RosterItemDto;
};
type DuelResponseDto = {
  duel: DuelDto | null;
  roster: RosterItemDto[];
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

  useEffect(() => {
    fetchNextDuel();
  }, []);

  async function fetchNextDuel() {
    setLoading(true);
    const res = await fetch(
      `https://localhost:7135/api/gauntlet/${gauntletId}/start`,
      {
        method: "POST",
      }
    );

    const data: DuelResponseDto = await res.json();
    setDuel(data.duel);
    setRoster(data.roster);

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
    const res = await fetch("https://localhost:7135/api/gauntlet/duel/submit", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        duelId: duel.duelId,
        winnerRosterItemId: winnerId,
      } as SubmitDuelRequest),
    });

    const data: DuelResponseDto = await res.json();
    setDuel(data.duel);
    setRoster(data.roster);

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

        {submitting && <p>Submitting choice...</p>}
      </div>
      <div className="overflow-y-auto">
        {roster && roster.length > 0 && <MiniCards items={roster} />}
      </div>
    </div>
  );
}
