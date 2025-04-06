import { Trophy, Award, Medal } from "lucide-react";
import Image from "next/image";
import { RosterItemDto } from "./Gauntlet";

export const MiniCards = ({ items }: { items: RosterItemDto[] }) => {
  const getRankIcon = (rank) => {
    switch (rank) {
      case 1:
        return <Trophy className="h-4 w-4 text-yellow-500" />;
      case 2:
        return <Award className="h-4 w-4 text-gray-400" />;
      case 3:
        return <Medal className="h-4 w-4 text-amber-700" />;
      default:
        return rank;
    }
  };
  //  // Sort items by score in descending order
  items.sort((a, b) => b.score - a.score);
  return (
    <div className="space-y-2">
      <div className="text-sm text-muted-foreground mb-2">
        Compact Mini List
      </div>
      {items.map((item, index) => (
        <div
          key={item.id}
          className="flex items-center gap-2 p-2 rounded-lg hover:bg-muted/50"
        >
          <div className="w-6 text-center font-bold text-sm text-muted-foreground">
            {index + 1 <= 3 ? getRankIcon(index + 1) : `#${index + 1}`}
          </div>
          <Image
            src={item.imageUrl || "/placeholder.svg"}
            alt={item.name}
            width={24}
            height={24}
            className="rounded-full"
          />
          <div className="flex-grow min-w-0">
            <div className="font-medium text-sm truncate">
              {item.name} ({new Date(item.details.releaseDate).getFullYear()})
            </div>
            <div className="text-xs text-muted-foreground">
              W: {item.wins} L: {item.losses}
            </div>
          </div>
          <div className="text-sm font-bold">{item.score}</div>
        </div>
      ))}
    </div>
  );
};
