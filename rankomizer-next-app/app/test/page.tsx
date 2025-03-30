import { Button } from "@/components/ui/button";
import { ThemeSwitcher } from "../ThemeSwitcher";

export default function Test() {
  return (
    <div className="">
      <Button>Click me</Button>
      <h1 className="text-3xl font-bold underline">Hello world!</h1>
      <ThemeSwitcher />
    </div>
  );
}
