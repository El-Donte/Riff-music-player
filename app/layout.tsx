import { Figtree } from "next/font/google";

import  Sidebar  from "@/components/Sidebar";
import "./globals.css";
import ModalProvider from "@/providers/ModalProvider";
import ToasterProvider from "@/providers/ToastProvider";
import UserProvider from "@/providers/UserProvide";
import getTracksByUserId from "@/actions/getTracksByUserId";

const font = Figtree({
  subsets: ["latin"],
});

export const metadata = {
  title: 'music-player',
  description: 'music-player'
}

export const revalidate = 0;

export default async function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  const userTracks = await getTracksByUserId();

  return (
    <html lang="en">
      <body className={`${font.className}`}>
        <UserProvider>
          <ToasterProvider/>
          <ModalProvider/>
          <Sidebar tracks = {userTracks}>
            {children}
          </Sidebar>
        </UserProvider>
      </body>
    </html>
  );
}
