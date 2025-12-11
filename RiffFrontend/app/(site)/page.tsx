import getTracks from "@/actions/getTracks";
import Header from "@/components/Header";
import ListItem from "@/components/Items/ListItem";
import PageContent from "./components/PageContent";

import { Suspense } from "react";

export const revalidate = 0;

export default async function Home() {
	return (
		<div className="
		bg-neutral-900
		rounded-lg
		h-full
		w-full
		overflow-hidden
		overflow-y-auto
		">
		<Header>
			<div className="mb-2">
				<h1 className="
					text-white
					text-3xl
					font-semibold
				">
					С возвращением
				</h1>
			</div>
			<div
				className="
					grid
					grid-cols-1
					sm:grid-cols-2
					xl:grid-cols-3
					2xl:grid-cols-4
					gap-3
					mt-4
				"
			>
				<ListItem
					image="/images/liked.png"
					name="Любимые треки"
					href="liked"
				/>
			</div>
		</Header>
		<div className="mt-2 mb-7 px-6">
			<div className="flex justify-between items-center">
				<h1 className="text-white text-2xl font-semibold">Новые треки</h1>
			</div>
			<Suspense fallback={<PageContent tracks={[]} loading={true} />}>
				<TracksLoader />
			</Suspense>
		</div>
		</div>
	);
}

async function TracksLoader() {
	const tracks = await getTracks();
	return <PageContent tracks={tracks} loading={false} />;
}