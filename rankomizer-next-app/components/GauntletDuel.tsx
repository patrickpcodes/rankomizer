import { DuelDto } from "./Gauntlet";

interface GauntletDuelProps {
  duel: DuelDto;
  handlePick: (id: string) => void;
  submitting: boolean;
}

export const GauntletDuel = ({
  duel,
  handlePick,
  submitting,
}: GauntletDuelProps) => {
  const [left, right] =
    // Math.random() > 0.5
    [duel.optionA, duel.optionB];
  // : [duel.optionB, duel.optionA];

  return (
    <div>
      <h2 className="text-xl font-bold">Which do you prefer?</h2>
      {submitting ? (
        <div className="flex">
          <p>Submitting Choice</p>
        </div>
      ) : (
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
      )}
    </div>
  );
};
