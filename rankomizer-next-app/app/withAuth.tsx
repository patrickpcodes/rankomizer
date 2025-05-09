"use client";
import { useEffect } from "react";
import { useRouter } from "next/navigation";
import { useAuth } from "@/app/authContext";

const withAuth = (WrappedComponent: React.ComponentType<any>) => {
  const ProtectedComponent: React.FC<any> = (props) => {
    const { user } = useAuth();
    const router = useRouter();

    useEffect(() => {
      if (!user) {
        router.push("/login");
      }
    }, [user, router]);

    if (!user) {
      return <p>Redirecting to login...</p>;
    }

    return <WrappedComponent {...props} />;
  };

  return ProtectedComponent;
};

export default withAuth;
