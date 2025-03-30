"use client";

// import { useEffect, useState } from "react";
import {
  ChevronDown,
  LogOut,
  //   Moon,
  PanelLeft,
  Settings,
  //   Sun,
  User,
} from "lucide-react";

import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  //   DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
// import { useAuth } from "./authContext";
import { useRouter } from "next/navigation";
import { ThemeSwitcher } from "./ThemeSwitcher";

export default function BrandedTopBar() {
  const user = null;
  //   const { user, logout } = useAuth();

  const router = useRouter();

  console.log("user", user);

  const handleLogout = async () => {
    // await logout();
  };

  return (
    <div className="bg-primary dark:bg-gray-900 text-primary-foreground dark:text-white dark:border-b dark:border-white">
      <div className="flex h-16 items-center px-4">
        <div className="flex items-center gap-3">
          <Button
            variant="ghost"
            size="icon"
            className="text-primary-foreground dark:text-white hover:bg-primary/90 dark:hover:bg-gray-800"
          >
            <PanelLeft className="h-5 w-5" />
            <span className="sr-only">Toggle sidebar</span>
          </Button>

          <div className="flex items-center gap-2">
            <div className="h-8 w-8 rounded-md bg-primary-foreground dark:bg-white flex items-center justify-center">
              <div className="h-4 w-4 rounded-sm bg-primary dark:bg-gray-900" />
            </div>
            <h2 className="text-lg font-bold">Rankomizer SAAS</h2>
          </div>
        </div>

        <div className="hidden md:flex mx-auto">
          <nav className="flex space-x-4">
            <Button
              variant="ghost"
              className="text-primary-foreground dark:text-white hover:bg-primary/90 dark:hover:bg-gray-800"
              onClick={() => router.push("/")}
            >
              Home
            </Button>
            <Button
              variant="ghost"
              className="text-primary-foreground dark:text-white hover:bg-primary/90 dark:hover:bg-gray-800"
            >
              Dashboard
            </Button>
            <Button
              variant="ghost"
              className="text-primary-foreground dark:text-white hover:bg-primary/90 dark:hover:bg-gray-800"
            >
              Projects
            </Button>
            <Button
              variant="ghost"
              className="text-primary-foreground dark:text-white hover:bg-primary/90 dark:hover:bg-gray-800"
            >
              Team
            </Button>
            <Button
              variant="ghost"
              className="text-primary-foreground dark:text-white hover:bg-primary/90 dark:hover:bg-gray-800"
              onClick={() => router.push("/marketplace-orders")}
            >
              Marketplace Orders
            </Button>
          </nav>
        </div>

        <div className="ml-auto flex items-center gap-2">
          {user != null ? (
            <>
              <Button
                variant="ghost"
                size="icon"
                className="text-primary-foreground dark:text-white hover:bg-primary/90 dark:hover:bg-gray-800"
              >
                <Settings className="h-5 w-5" />
                <span className="sr-only">Settings</span>
              </Button>

              <DropdownMenu>
                <DropdownMenuTrigger asChild>
                  <div className="flex items-center gap-2 rounded-full bg-primary-foreground/10 dark:bg-white/10 px-2 py-1 cursor-pointer hover:bg-primary-foreground/20 dark:hover:bg-white/20">
                    <Avatar className="h-7 w-7">
                      <AvatarImage
                        src="/placeholder.svg?height=28&width=28"
                        alt="User"
                      />
                      <AvatarFallback className="bg-primary-foreground/20 dark:bg-white/20 text-primary-foreground dark:text-white text-xs">
                        JD
                      </AvatarFallback>
                    </Avatar>
                    <span className="hidden md:inline text-sm font-medium">
                      {/* {user.username} */}
                    </span>
                    <ChevronDown className="h-4 w-4" />
                  </div>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end">
                  <DropdownMenuItem
                    onClick={() => console.log("clicked on my account")}
                  >
                    My Account
                  </DropdownMenuItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem>
                    <User className="mr-2 h-4 w-4" />
                    <span>Profile</span>
                  </DropdownMenuItem>
                  <DropdownMenuItem>
                    <Settings className="mr-2 h-4 w-4" />
                    <span>Settings</span>
                  </DropdownMenuItem>
                  <DropdownMenuSeparator />
                  <DropdownMenuItem onClick={() => handleLogout()}>
                    <LogOut className="mr-2 h-4 w-4" />
                    <span>Log out</span>
                  </DropdownMenuItem>
                </DropdownMenuContent>
              </DropdownMenu>
            </>
          ) : (
            <>
              <Button
                variant="ghost"
                //   onClick={toggleLogin}
                className="text-primary-foreground dark:text-white hover:bg-primary/90 dark:hover:bg-gray-800"
              >
                Log in
              </Button>
              <Button
                variant="secondary"
                className="dark:bg-white dark:text-gray-900 dark:hover:bg-gray-200"
              >
                Sign up
              </Button>
              <Button
                variant="ghost"
                size="icon"
                className="text-primary-foreground dark:text-white hover:bg-primary/90 dark:hover:bg-gray-800"
              >
                <Settings className="h-5 w-5" />
                <span className="sr-only">Settings</span>
              </Button>
              <ThemeSwitcher />
            </>
          )}
        </div>
      </div>
    </div>
  );
}
