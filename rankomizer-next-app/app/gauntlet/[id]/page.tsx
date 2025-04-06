import GauntletDuel from "@/components/GauntletDuel";

export default function GauntletPage({ params }: { params: { id: string } }) {
  return (
    <main className="p-8">
      <GauntletDuel gauntletId={params.id} />
    </main>
  );
}
