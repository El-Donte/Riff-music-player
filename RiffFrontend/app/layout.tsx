import { Figtree } from "next/font/google";

import "./globals.css";
import  Sidebar  from "@/components/Sidebar";
import ModalProvider from "@/providers/ModalProvider";
import ToasterProvider from "@/providers/ToastProvider";
import UserProvider from "@/providers/UserProvide";
import getTracksByUserId from "@/actions/getTracksByUserId";
import Player from "@/components/MusicPlayer/Player";

import { Suspense } from "react";

const font = Figtree({
  subsets: ["latin"],
});

export const metadata = {
  title: 'Riff-music-player',
  description: 'Riff-music-player'
}

export default async function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body className={`${font.className}`}>
        <UserProvider>
          <ToasterProvider/>
          <ModalProvider/>
          <Suspense fallback={<Sidebar tracks={[]} loading={true}>{children}</Sidebar>}>
            <TracksLoader>{children}</TracksLoader>
          </Suspense>
          <Player/>
        </UserProvider>
      </body>
    </html>
  );
}

async function TracksLoader({ children }: { children: React.ReactNode }){
	const userTracks = await getTracksByUserId();
	return (
    <Sidebar tracks={userTracks} loading={false}>
        {children}
    </Sidebar>
  );
}
