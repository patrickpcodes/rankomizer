"use client";

import React, { useEffect, useState } from "react";

type ItemType = "Movie" | "Song" | "Painting";

type DuelDto = {
  duelId: string;
  optionA: RosterItemDto;
  optionB: RosterItemDto;
  roster: RosterItemDto[];
};

type RosterItemDto = {
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

export default function GauntletDuel({ gauntletId }: GauntletDuelProps) {
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

    const data = await res.json();
    setLoading(false);

    if (!data) {
      setFinished(true);
    } else {
      setDuel(data);
      setRoster(data.roster);
    }
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

    const data = await res.json();
    setSubmitting(false);

    if (!data) {
      setDuel(null);
      setRoster([]);
      setFinished(true);
    } else {
      setOrder(Math.random() < 0.5);
      setDuel(data);
      setRoster(data.roster);
    }
  }

  if (loading) return <p>Loading duel...</p>;
  if (finished) return <p>🔥 Gauntlet complete!</p>;
  if (!duel) return <p>Nothing to show</p>;

  const [left, right] = order
    ? [duel.optionA, duel.optionB]
    : [duel.optionB, duel.optionA];

  return (
    <div className="flex flex-row items-center justify-center gap-6">
      <div>
        <h2 className="text-xl font-bold">Which do you prefer?</h2>

        <div className="flex gap-8">
          {[left, right].map((option) => (
            <div
              key={option.id}
              className="cursor-pointer transition hover:scale-105"
              onClick={() => !submitting && handlePick(option.id)}
            >
              <img
                src={option.imageUrl}
                alt={option.name}
                className="w-64 h-96 object-cover rounded-xl shadow-md"
              />
              <p className="text-center mt-2 font-medium">{option.name}</p>
            </div>
          ))}
        </div>
      </div>
      <div>
        {roster && roster.length > 0 && (
          <div className="mt-10 w-full max-w-3xl">
            <h3 className="text-lg font-semibold mb-2">Current Standings</h3>
            <table className="w-full border text-left text-sm">
              <thead>
                <tr>
                  <th className="py-1 px-2">Name</th>
                  <th className="py-1 px-2 text-center">Wins</th>
                  <th className="py-1 px-2 text-center">Losses</th>
                  <th className="py-1 px-2 text-center">Score</th>
                </tr>
              </thead>
              <tbody>
                {roster.map((item) => (
                  <tr key={item.id}>
                    <td className="py-1 px-2">{item.name}</td>
                    <td className="py-1 px-2 text-center">{item.wins}</td>
                    <td className="py-1 px-2 text-center">{item.losses}</td>
                    <td className="py-1 px-2 text-center">
                      {item.score.toFixed(2)}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      {submitting && <p>Submitting choice...</p>}
    </div>
  );
}
