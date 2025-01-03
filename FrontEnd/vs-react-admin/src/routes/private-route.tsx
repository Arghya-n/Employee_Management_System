import { useAppSelector } from "@/store";
import React from "react";
import { Navigate, useLocation } from "react-router-dom";

interface PrivateRouteProps {
  children: React.ReactNode;
  isAuthenticated: boolean;
}

const PrivateRoute = ({ children, isAuthenticated }: PrivateRouteProps) => {
  const location = useLocation();
  const userRole = useAppSelector((state) => state.user?.role || "customer");
  const currentPath =
    location.pathname !== "/"
      ? `?redirect=${encodeURIComponent(location.pathname)}${encodeURIComponent(location.search)}`
      : "";
  const redirectUrl = `/login${currentPath}`;

  const restrictedRoutesForCustomer = ["/users", "/tasks", "/taskAssignment"];

  if (
    userRole === "customer" &&
    restrictedRoutesForCustomer.includes(location.pathname)
  ) {
    // Redirect customers to the dashboard if they access restricted routes
    return <Navigate to="/" />;
  }

  return isAuthenticated ? children : <Navigate to={redirectUrl} />;
};

export default PrivateRoute;
